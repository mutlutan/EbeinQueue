namespace EbeninQueue
{
    #region models
    public class MoResponse<T> where T : new()
    {
        public bool IsSuccess { get; set; } = false;
        public List<string> Messages { get; set; } = new List<string>();
        public T? Data { get; set; }
    }


    public class MoEnqueue
    {
        public string Channel { get; set; } = "";
        public string Element { get; set; } = "";
        public int Periority { get; set; } = 0;
    }

    public class MoInput
    {
        public string Channel { get; set; } = "";
    }
    #endregion

}
