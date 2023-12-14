using System;
using Dudoan.Dbcontext;
using Dudoan.MML.Interface;
using Dudoan.Model;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Trainers.Recommender;
using Microsoft.ML.Transforms;

namespace Dudoan.MML;

public class MLService : IMLService
{
    private readonly IServiceProvider serviceProvider;
    private MLContext mLContext;
    private TransformerChain<MatrixFactorizationPredictionTransformer> model;

    public MLService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public void loadFromDB()
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = mLContext.Data.LoadFromEnumerable(dbContext.Videos.Select(x => new MLModel
        {
            Id = x.Id,
            Title = x.Title,
            Tags = string.Join(",", x.Tags.Select(y => y.Name)),
            Likes = x.Likes.Select(like => new MLModel.Like()
            {
                UserId = like.UserId,
                VideoId = like.VideoId,
                LikeStatus = like.LikeStatus,
                Time = like.Time
            }).ToList(),
            Watches = x.Watches.Select(watch => new MLModel.Watch()
            {
                UserId = watch.User.Id,
                VideoId = watch.Video.Id,
                Duration = watch.Details.Select(detail => new MLModel.DurationType()
                {
                    Duration = detail.Duration,
                    When = detail.When
                }).ToList()
            }).ToList()
        }));
        Train(data);
        mLContext.Model.Save(model, data.Schema, Path.Combine(Environment.CurrentDirectory, "Data", "VideoModel.zip"));
    }

    public void Train(IDataView data)
    {
        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = "VideoId0",
            MatrixRowIndexColumnName = "VideoId1",
            ApproximationRank = 100,
            NumberOfIterations = 20,
            NumberOfThreads = 6,
            LabelColumnName = "Ratio"
        };
        var pipeline = mLContext.Transforms.Conversion.MapKeyToValue(nameof(Video.Id), nameof(Video.Id))
            .Append(mLContext.Transforms.Text.FeaturizeText("Tags", nameof(Video.Tags)))
            .Append(mLContext.Transforms.Conversion.MapValueToKey("TagsKey", "Tags"))
            .Append(mLContext.Transforms.Categorical.OneHotEncoding("TagsKey"))
            .Append(mLContext.Transforms.NormalizeMinMax("Tags"))
            .Append(mLContext.Transforms.Conversion.MapValueToKey("Likes", nameof(Video.Likes)))
            .Append(mLContext.Transforms.Conversion.MapValueToKey("Watches", nameof(Video.Watches)))
            .Append(mLContext.Recommendation().Trainers.MatrixFactorization(options));
        model = pipeline.Fit(data);
    }

    public class MatrixStruct
    {
        public Guid VideoId0 { get; set; }
        public Guid VideoId1 { get; set; }
        public float Ratio { get; set; }

    }

    public List<Guid> Recommendation(MLModel input)
    {
        var inputVideoData = mLContext.Data.LoadFromEnumerable(new List<MLModel> { input });
        var predictions = model.Transform(inputVideoData);
        var predictionEnumerable = mLContext.Data.CreateEnumerable<MatrixStruct>(predictions, reuseRowObject: false);
        var videos = predictionEnumerable.OrderByDescending(prediction => prediction.Ratio)
    .Take(5)
    .Select(prediction => prediction.VideoId1);
        return videos.ToList();
    }



}