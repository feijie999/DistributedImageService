namespace ImageApi.Core
{
    public interface IImageParameterFixer
    {
        ImageParameter Fix(ImageParameter parameter);
    }
}