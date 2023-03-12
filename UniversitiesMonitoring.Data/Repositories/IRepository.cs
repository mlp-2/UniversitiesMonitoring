namespace UniversityMonitoring.Data.Repositories;

public interface IRepository<TEntity, in TKey> where TEntity : class
{
    /// <summary>
    /// Добавляет новую запись в БД
    /// </summary>
    /// <param name="entity">Запись</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Находит запись в БД в соответствии с ключом
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Запись. Если ее нет в БД, то возвращается null</returns>
    Task<TEntity?> FindAsync(TKey key);

    /// <summary>
    /// Удаляет запись из БД
    /// </summary>
    /// <param name="entity">Запись, которую нужно убрать</param>
    void Remove(TEntity entity);

    /// <summary>
    /// Выполняет SQL запрос
    /// </summary>
    /// <param name="sql">SQL</param>
    IEnumerable<TEntity> ExecuteSql(string sql);

    /// <summary>
    /// Получает все сущности
    /// </summary>
    IQueryable<TEntity> GetlAll();
}