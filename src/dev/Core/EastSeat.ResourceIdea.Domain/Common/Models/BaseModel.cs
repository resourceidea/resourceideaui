namespace EastSeat.ResourceIdea.Domain.Common.Models
{
    /// <summary>
    /// Base model for all models.
    /// </summary>
    public abstract record BaseModel<T> where T : class
    {
        /// <summary>Default value for all models. </summary>
        public abstract T DefaultInstance { get; }
    }
}