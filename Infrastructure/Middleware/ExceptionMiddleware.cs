using Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsJsonAsync(new
                {
                    errors = ex.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                    })
                });
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = 404;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
