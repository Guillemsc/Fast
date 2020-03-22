using System;
using System.Threading.Tasks;

namespace Fast
{
    public class AwaitRequest<S, E>
    {
        protected bool has_errors = false;
        protected S success_result = default(S);
        protected E error_result = default(E);

        protected readonly Callback<RequestProgress> on_progress_update = new Callback<RequestProgress>();

        public bool HasErrors => has_errors;
        public S SuccessResult => success_result;
        public E ErrorResult => error_result;

        public async Task RunRequest()
        {
            await RunRequestInternal();
        }

        public Callback<RequestProgress> OnProgressUpdate
        {
            get { return on_progress_update; }
        }

        protected void UpdateProgress(int progress_index, float progress_value)
        {
            RequestProgress ret = new RequestProgress(progress_index, progress_value);
            on_progress_update.Invoke(ret);
        }

        protected void UpdateProgress(string progress_description, float progress_value)
        {
            RequestProgress ret = new RequestProgress(progress_description, progress_value);
            on_progress_update.Invoke(ret);
        }

        protected virtual async Task RunRequestInternal()
        {
            
        }
    }
}
