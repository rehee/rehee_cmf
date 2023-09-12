namespace ReheeCmf.Helpers
{
  public static class ValidationResultHelper
  {
    public static ValidationResult New(string message, params string[] keys)
    {
      return new ValidationResult(message, keys);
    }
  }
}
