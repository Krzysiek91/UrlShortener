namespace UrlShortener.Validators
{
    public interface IValidator<T>
    {
        public void IsValid(T value);
    }
}
