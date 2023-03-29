using Dapper;
using System.Reflection;

namespace DataAccessLayer.CustomColumnAttribute
{
    public class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            throw new NotImplementedException();
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (SqlMapper.ITypeMap mapper in _mappers)
            {
                try
                {
                    SqlMapper.IMemberMap result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {

                }
            }
            return null!;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (SqlMapper.ITypeMap mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {

                }
            }
            return null!;
        }

        public ConstructorInfo? FindExplicitConstructor()
        {
            return _mappers
                .Select(mapper => mapper.FindExplicitConstructor())
                .FirstOrDefault(result => result != null);
        }
    }
}
