using Microsoft.ML.Data;

namespace Dudoan.ML;
public class MovieRating
{
    [LoadColumn(0)]
    public float userId;
    [LoadColumn(1)]
    public float movieId;
    [LoadColumn(2)]
    public float Label;
}
// </SnippetMovieRatingClass>

// <SnippetPredictionClass>
public class MovieRatingPrediction
{
    public float Label;
    public float Score;
}