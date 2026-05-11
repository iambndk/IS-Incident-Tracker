using System.Linq.Expressions;

namespace ISIncidentTracker.Web.Data.Repositories.Interfaces;

/// <summary>
/// Базовый интерфейс репозитория для операций CRUD.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Получает сущность по идентификатору.
    /// </summary>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Получает все сущности.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Получает сущности по условию.
    /// </summary>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Добавляет новую сущность.
    /// </summary>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Обновляет существующую сущность.
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Удаляет сущность по идентификатору.
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Сохраняет изменения в базе данных.
    /// </summary>
    Task<int> SaveChangesAsync();
}
