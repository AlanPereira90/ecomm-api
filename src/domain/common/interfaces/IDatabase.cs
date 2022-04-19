namespace src.domain.common.interfaces;

public interface IDatabase<T>
{
  Task<T> Save(T entity);
}
