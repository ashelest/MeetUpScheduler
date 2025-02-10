namespace Application.Interfaces;

public interface ICachingService
{
	Task SetAsync<TKey, TValue>(TKey key, TValue value);
	Task<TValue?> GetAsync<TKey, TValue>(TKey key) where TValue : class;
}