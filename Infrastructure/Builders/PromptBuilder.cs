namespace Infrastructure.Builders
{
    public static class PromptBuilder
    {
        public static object Build(
            string userContext,
            string message)
        {
            return new
            {
                model = "meta-llama/Llama-3.1-8B-Instruct",

                messages = new[]
                {
                new
                {
                    role = "system",
                    content =
                    "You are a financial AI assistant inside FinancesManager. " +
                    "Only answer financial questions."
                },
                new
                {
                    role = "system",
                    content = userContext
                },
                new
                {
                    role = "user",
                    content = message
                }
            }};
        }
    }
}
