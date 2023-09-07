﻿using ReheeCmf.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReheeCmf.Helper
{
  public static class ContentResponseHelper
  {
    public static IContentResponse SetSuccess(this IContentResponse response, object? contentResponse)
    {
      response.ContentObject = contentResponse;
      response.Success = true;
      return response;
    }
    public static IContentResponse<T> SetSuccess<T>(this IContentResponse<T> response, T? contentResponse)
    {
      response.Content = contentResponse;
      response.Success = true;
      return response;
    }
    public static void SetMultiResponse<T>(this IContentResponse<IEnumerable<T>> that, IEnumerable<IContentResponse<T>> sources)
    {
      that.Success = sources.All(b => b.Success);
      that.Content = sources.Where(b => b.Content != null).Select(b => b.Content!);
      if (!that.Success)
      {
        that.ErrorMessage = String.Join(",", sources.Select(b => b.ErrorMessage));
      }
    }

    public static T SetError<T>(this T that, Exception exception, HttpStatusCode status = HttpStatusCode.InternalServerError) where T : IContentResponse
    {
      if (exception is StatusException statusEx)
      {
        that.ExceptionValidation = statusEx.ValidationError?.Select(b => b);
      }
      return that.SetError(status, exception.Message);
    }
    public static T SetNotFound<T>(this T that, string? message = null) where T : IContentResponse
    {
      return that.SetError(HttpStatusCode.NotFound, message);
    }
    public static T SeNull<T>(this T that) where T : IContentResponse
    {
      return that.SetError(HttpStatusCode.BadRequest, "null instance");
    }
    public static T SetError<T>(this T that, IContentResponse error) where T : IContentResponse
    {
      return that.SetError(error.Status, error.ErrorMessage, error.ErrorCode);
    }

    public static T SetError<T>(this T error, HttpStatusCode status = HttpStatusCode.BadRequest, string? errorMessage = null, string? errorCode = null) where T : IContentResponse
    {
      error.Status = status;
      error.ErrorCode = errorCode;
      error.ErrorMessage = errorMessage;
      error.Success = false;
      return error;
    }

    public static IContentResponse SetSuccess<T>(this IContentResponse<T> that, T input, HttpStatusCode status = HttpStatusCode.OK)
    {
      that.Content = input;
      that.Status = status;
      that.Success = true;
      return that;
    }

  }
}
