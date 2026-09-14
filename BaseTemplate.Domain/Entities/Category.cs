namespace BaseTemplate.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public readonly List<Product> _products = new();

        protected Category() { }

        public Category(string name)
        {
            SetName(name);
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome da categoria não pode ser vazio.", nameof(name));

            Name = name;
        }
    }
}
