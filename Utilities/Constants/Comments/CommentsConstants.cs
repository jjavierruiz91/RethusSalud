namespace rethus_backend.Utilities.Constants.User.CommentsConstants
{
    public enum CommentsStatus
    {
        pending,
        approved,
        rejected
    }

    public enum CommentsType
    {
        Internal, //Son los comentarios que solo se muestran a los funcionarios
        External, // Son los comentarios que solo se muestran a los
        Both // Son los comentarios que se veran tanto para el interno como extenro
    }
}
