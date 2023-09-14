namespace ReheeCmf.Helpers
{
  public static class ValidationResultHelper
  {
    public static ValidationResult New(string message, params string[] keys)
    {
      return new ValidationResult(message, keys);
    }
    public static ContentResponse<bool> ValidationrResponse(this object obj)
    {
      var result = new ContentResponse<bool>();
      if (obj == null)
      {
        return result;
      }
      if (obj is IValidatableObject vb)
      {
        var validation = vb.Validate(new ValidationContext(obj));
        result.SetValidation(validation.ToArray());
      }
      else
      {
        var list = new List<ValidationResult>();
        Validator.TryValidateObject(obj, new ValidationContext(obj), list);
        result.SetValidation(list.ToArray());
      }
      return result;
    }
  }
}
