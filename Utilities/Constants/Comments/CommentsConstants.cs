namespace rethus_backend.Utilities.Constants.User.CommentsConstants
{
    public enum CommentsStatus
    {
        pending, // pendiente por revision de parte del usuario
        approved, // aprobado por el usuario o funcionario
        rejected, // Rechazado por el funcionario
        updated // Actualizado por el usuario
    }

    public enum CommentsType
    {
        Internal, //Son los comentarios que solo se muestran a los funcionarios
        External, // Son los comentarios que solo se muestran a los
        Both // Son los comentarios que se veran tanto para el interno como extenro
    }
}
