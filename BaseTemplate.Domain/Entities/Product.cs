namespace BaseTemplate.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category? Category { get; private set; }

        protected Product() { }

        public Product(string name, decimal price, Guid categoryId)
        {
            SetName(name);
            SetPrice(price);
            CategoryId = categoryId;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
                throw new ArgumentException("O preço do produto não pode ser negativo.", nameof(price));

            Price = price;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do produto não pode ser vazio.", nameof(name));

            Name = name;
        }

        public void UpdateAuditDate() => UpdatedAt = DateTime.UtcNow;
    }
}
