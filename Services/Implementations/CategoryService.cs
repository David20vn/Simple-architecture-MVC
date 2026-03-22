public void Delete(int id)
{
    var category = _categoryRepository.GetById(id);

    if (category == null)
        throw new Exception("Categoría no encontrada");

    var hasProducts = InMemoryData.Products.Any(p => p.CategoryId == id);

    if (hasProducts)
        throw new Exception("No se puede eliminar la categoría porque tiene productos asociados");

    _categoryRepository.Delete(id);
}