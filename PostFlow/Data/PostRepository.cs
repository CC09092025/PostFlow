using SQLite;
using PostFlow.Models;

namespace PostFlow.Data
{
    public class PostRepository
    {
        private SQLiteAsyncConnection _database;

        public PostRepository()
        {
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            if (_database != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "posts.db");
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<Post>();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            try
            {
                return await _database.Table<Post>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving posts: {ex.Message}");
                return new List<Post>();
            }
        }

        public async Task<int> SaveAllPostsAsync(List<Post> posts)
        {
            try
            {
                await ClearAllPostsAsync();
                return await _database.InsertAllAsync(posts);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting posts: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> ClearAllPostsAsync()
        {
            try
            {
                return await _database.DeleteAllAsync<Post>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting posts: {ex.Message}");
                return 0;
            }
        }
    }
}