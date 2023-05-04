namespace backend.Models
{
    public interface IGenericModel<T>
    {
        public T Id { set; get; }
    }
}
