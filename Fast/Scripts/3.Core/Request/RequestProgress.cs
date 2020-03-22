using System;

namespace Fast
{
    public class RequestProgress
    {
        private readonly int progress_index = 0;
        private readonly string progress_description = "";
        private readonly float progress_value = 0.0f;

        public RequestProgress(int progress_index, float progress_value)
        {
            this.progress_index = progress_index;
            this.progress_value = progress_value;
        }

        public RequestProgress(string progress_description, float progress_value)
        {
            this.progress_description = progress_description;
            this.progress_value = progress_value;
        }

        public int ProgressIndex => progress_index;
        public string ProgressDescription => progress_description;
        public float ProgressValue => progress_value;
    }
}
