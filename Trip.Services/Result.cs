namespace Trip.Service
{
    public class Result
    {
        public List<string> Errors { get; set; } = new();
        public bool Succeed { get; set; }

        public static Result Success()
        {
            return new Result
            {
                Succeed = true,
            };
        }

        public static Result Fail()
        {
            return new Result
            {
                Succeed = false,
            };
        }

        public static Result Fail(string error)
        {
            return new Result
            {
                Errors = new List<string> { error },
                Succeed = false,
            };
        }

        public static Result Fail(IEnumerable<string> errors)
        {
            return new Result
            {
                Errors = errors.ToList(),
                Succeed = false,
            };
        }
    }

    public class Result<TResult> : Result
    {
        public TResult? Data { get; set; }

        public static Result<TResult> Success()
        {
            return new Result<TResult>
            {
                Succeed = true,
            };
        }

        public static Result<TResult> Success(TResult data)
        {
            return new Result<TResult>
            {
                Succeed = true,
                Data = data,
            };
        }

        public static Result<TResult> Fail()
        {
            return new Result<TResult>
            {
                Succeed = false,
            };
        }

        public static Result<TResult> Fail(string error)
        {
            return new Result<TResult>
            {
                Errors = new List<string> { error },
                Succeed = false,
            };
        }

        public static Result<TResult> Fail(IEnumerable<string> errors)
        {
            return new Result<TResult>
            {
                Errors = errors.ToList(),
                Succeed = false,
            };
        }
    }
}
