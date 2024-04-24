// Programmer name : S Nondwatyu
// Student nr : 220036624
// Assignment nr : GA1
// Purpose : The purpose of this IDBInitializer interface is to define a contract
// for initializing the database context in an ASP.NET Core application.
// It declares a single method Initialize, which takes a SQLiteDBContext instance as a parameter
// and is responsible for initializing the database schema and seeding initial data.


using ASPNETCore_DB.Data;

namespace ASPNETCore_DB.Interfaces
{
    public interface IDBInitializer
    {
        void Initialize(SQLiteDBContext context);
    }
}
