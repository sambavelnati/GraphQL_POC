using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace EFCore3Sample
{
    public partial class DALBase : DbContext
    {
        public virtual DbSet<AppConfig> AppConfig { get; set; }
    }

    public sealed class AppConfigDAL
    {


        // Instance fields
        private readonly DALBase _db;


        /// <summary>
        /// Initializes a new instance of the AppConfig class.
        /// </summary>
        public AppConfigDAL()
        {
            _db = new DALBase();
        }

        /// <summary>
        /// Initializes a new instance of the AppConfig class.
        /// </summary>
        public AppConfigDAL(DALBase db)
        {
            _db = db ?? throw new InvalidOperationException("DALBase parameter cannot be null.");
        }


        /// <summary>
        /// Saves a record to the AppConfig table.
        /// </summary>
        public void Insert(AppConfig dto)
        {
            if (dto == null) { throw new InvalidOperationException("AppConfig parameter cannot be null"); }
            _db.AppConfig.Add(dto);

            _db.SaveChanges();
        }

        /// <summary>
        /// Saves a list of records to the AppConfig table.
        /// </summary>
        public void InsertRecords(List<AppConfig> listDto)
        {
            if (listDto == null) { throw new InvalidOperationException("List<AppConfig> parameter cannot be null"); }
            foreach (AppConfig dto in listDto)
            {
                _db.AppConfig.Add(dto);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// Updates a record in the AppConfig table.
        /// </summary>
        public void Update(AppConfig dto)
        {
            if (dto == null) { throw new InvalidOperationException("AppConfig parameter cannot be null"); }

            AppConfig dtoToBeSaved = _db.AppConfig.Find(dto.version, dto.ConfigKey);
            _db.Entry(dtoToBeSaved).CurrentValues.SetValues(dto);
            _db.Entry(dtoToBeSaved).State = EntityState.Modified;

            _db.SaveChanges();
        }

        /// <summary>
        /// Deletes a single record from the AppConfig table.
        /// </summary>
        public void Delete(int version, string ConfigKey)
        {
            // If there are string type parameters, null checks can be performed
            _db.AppConfig.Remove(_db.AppConfig.Find(version, ConfigKey));

            _db.SaveChanges();

        }

        /// <summary>
        /// Deletes a list of records from the AppConfig table.
        /// </summary>
        public void DeleteRecords(List<AppConfig> listDto)
        {
            if (listDto == null) { throw new InvalidOperationException("List<AppConfig> parameter cannot be null"); }

            foreach (AppConfig dto in listDto)
            {
                _db.AppConfig.Remove(dto);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// Deactivates a single record from the AppConfig table.
        /// </summary>
        public void Deactivate(string userId, int version, string ConfigKey)
        {

            if (userId == null) { throw new InvalidOperationException("userId parameter cannot be null"); }
            AppConfig dto = _db.AppConfig.Find(version, ConfigKey);
            dto.ModTime = DateTime.Now;
            dto.ModID = userId;
            dto.IsActive = "N";
            _db.AppConfig.Add(dto);
            _db.Entry(dto).State = EntityState.Modified;
            _db.SaveChanges();


        }

        public void Deactivate(DateTime modTime, string userId, int version, string ConfigKey)
        {

            if (userId == null) { throw new InvalidOperationException("userId parameter cannot be null"); }
            AppConfig dto = _db.AppConfig.Find(version, ConfigKey);
            dto.ModTime = DateTime.Now;
            dto.ModID = userId;
            dto.IsActive = "N";
            _db.AppConfig.Add(dto);
            _db.Entry(dto).State = EntityState.Modified;
            _db.SaveChanges();


        }

        /// <summary>
        /// Deactivates a list of records from the AppConfig table.
        /// </summary>
        public void DeactivateRecords(string userId, List<AppConfig> listDto)
        {
            if (userId == null) { throw new InvalidOperationException("userId parameter cannot be null"); }
            if (listDto == null) { throw new InvalidOperationException("List<AppConfig> parameter cannot be null"); }
            AppConfig dtoToDeact;
            foreach (var dto in listDto)
            {
                dtoToDeact = _db.AppConfig.Find(dto.version, dto.ConfigKey);
                dtoToDeact.ModTime = DateTime.Now;
                dtoToDeact.ModID = userId;
                dtoToDeact.IsActive = "N";


                _db.AppConfig.Add(dtoToDeact);

                _db.Entry(dtoToDeact).State = EntityState.Modified;
            }
            _db.SaveChanges();

        }


        /// <summary>
        /// Selects a single record from the AppConfig table.
        /// </summary>
        public AppConfig Select(int version, string ConfigKey)
        {
            AppConfig dto = _db.AppConfig.Find(version, ConfigKey);

            return dto;
        }

        /// <summary>
        /// Selects all records from the AppConfig table by search object.
        /// </summary>
        public List<AppConfig> SearchRecords(AppConfigSO dtoSO, string orderBy)
        {

            if (orderBy == null) orderBy = "";

            List<AppConfig> listDto = new List<AppConfig>();

            var dbDto = (from records in _db.AppConfig select records);

            dbDto = AddFilter(dbDto, dtoSO);

            dbDto = AddOrderBy(dbDto, orderBy);

            listDto = dbDto.ToList();

            return listDto;
        }


        public List<AppConfig> SearchPagedRecords(AppConfigSO dtoSO, string orderBy, int startIndex, int recordsPerPage)
        {

            if (orderBy == null) orderBy = "";

            List<AppConfig> listDto = new List<AppConfig>();

            var dbDto = (from records in _db.AppConfig select records);

            dbDto = AddFilter(dbDto, dtoSO);

            dbDto = AddOrderBy(dbDto, orderBy);

            listDto = dbDto
                            .Skip(startIndex)
                            .Take(recordsPerPage)
                            .ToList();

            return listDto;
        }

        public int GetTotalCount(AppConfigSO dtoSO)
        {

            var dbDto = (from records in _db.AppConfig select records);

            dbDto = AddFilter(dbDto, dtoSO);

            return dbDto.Count();
        }

        public Dictionary<string, string> GetKeyValuesSO(string keyCol, string valueCol, AppConfigSO so, string orderBy)
        {
            if (keyCol == null) { throw new InvalidOperationException("keyCol parameter cannot be null"); }
            if (valueCol == null) { throw new InvalidOperationException("valueCol parameter cannot be null"); }
            if (orderBy == null) orderBy = "";

            Dictionary<string, string> keyValPair = new Dictionary<string, string>();
            var dbDto = (from records in _db.AppConfig select records);
            dbDto = AddFilter(dbDto, so);
            dbDto = AddOrderBy(dbDto, orderBy, valueCol);
            IQueryable result;

            if (keyCol == valueCol)
            {
                result = dbDto.Select("new (" + keyCol + ")");
            }
            else
            {
                result = dbDto.Select("new (" + keyCol + "," + valueCol + ")");
            }
            string a = "";
            foreach (dynamic d in result)
            {
                if (!keyValPair.TryGetValue(d.GetType().GetProperty(keyCol).GetValue(d).ToString(), out a))
                    keyValPair.Add(d.GetType().GetProperty(keyCol).GetValue(d).ToString(),
                        d.GetType().GetProperty(valueCol).GetValue(d).ToString());
            }

            return keyValPair;
        }

        // This method is deprecated , above method GetKeyValuesSO() should be used for drop down binding
        public Dictionary<string, string> GetKeyValuesSQL(string keyCol, string valueCol, string whereSQL, string orderBy)
        {
            if (keyCol == null) { throw new InvalidOperationException("keyCol parameter cannot be null"); }
            if (valueCol == null) { throw new InvalidOperationException("valueCol parameter cannot be null"); }
            if (whereSQL == null) whereSQL = "";
            if (orderBy == null) orderBy = "";

            Dictionary<string, string> keyValPair = new Dictionary<string, string>();
            var dbDto = (from records in _db.AppConfig select records);
            dbDto = AddFilter(dbDto, whereSQL);
            dbDto = AddOrderBy(dbDto, orderBy, valueCol);
            IQueryable result;

            if (keyCol == valueCol)
            {
                result = dbDto.Select("new (" + keyCol + ")");
            }
            else
            {
                result = dbDto.Select("new (" + keyCol + "," + valueCol + ")");
            }
            string a = "";
            foreach (dynamic d in result)
            {
                if (!keyValPair.TryGetValue(d.GetType().GetProperty(keyCol).GetValue(d).ToString(), out a))
                    keyValPair.Add(d.GetType().GetProperty(keyCol).GetValue(d).ToString(),
                        d.GetType().GetProperty(valueCol).GetValue(d).ToString());
            }

            return keyValPair;
        }

        public T GetAggregateValue<T>(AggregateFunction aggregateFunction, string aggrColName, AppConfigSO so)
        {
            if (aggrColName == null) { throw new InvalidOperationException("aggrColName parameter cannot be null"); }

            T aggrVal = default(T);
            var dbDto = (from records in _db.AppConfig select records);
            dbDto = AddFilter(dbDto, so);
            var result = dbDto.GroupBy("new (1 as GROUPCOL)", "new (" + aggrColName + ")");
            result = result
                .Select("new (" + AggregateFunctionGetter.GetLINQFunction(aggregateFunction)
                + "(" + aggrColName + ") as " + aggrColName + ")");

            foreach (dynamic d in result)
            {
                aggrVal += d.GetType().GetProperty("" + aggrColName + "").GetValue(d);
            }

            return aggrVal;
        }

        private IQueryable<AppConfig> AddFilter(IQueryable<AppConfig> dbDto, AppConfigSO dtoSO)
        {
            if (dtoSO != null)
            {
                if (dtoSO.versionArr != null && dtoSO.versionArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.versionArr.Contains(x.version));
                }
                else if ((dtoSO.version1 != 0) && (dtoSO.version2 != 0))
                {
                    dbDto = dbDto
                        .Where(x => x.version >= dtoSO.version1 && x.version <= dtoSO.version2);
                }
                else if (dtoSO.version1 != 0)
                {
                    dbDto = dbDto
                        .Where(x => x.version >= dtoSO.version1);
                }
                else if (dtoSO.version2 != 0)
                {
                    dbDto = dbDto
                        .Where(x => x.version <= dtoSO.version2);
                }
                if (dtoSO.ConfigKeyArr != null && dtoSO.ConfigKeyArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.ConfigKeyArr.Contains(x.ConfigKey));
                }
                else if (!string.IsNullOrEmpty(dtoSO.ConfigKey))
                {
                    dbDto = dbDto
        .Where(x => x.ConfigKey.ToUpper().StartsWith(dtoSO.ConfigKey.ToUpper()));
                }
                if (dtoSO.ConfigValueArr != null && dtoSO.ConfigValueArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.ConfigValueArr.Contains(x.ConfigValue));
                }
                else if (!string.IsNullOrEmpty(dtoSO.ConfigValue))
                {
                    dbDto = dbDto
        .Where(x => x.ConfigValue.ToUpper().StartsWith(dtoSO.ConfigValue.ToUpper()));
                }
                if (dtoSO.RmrkArr != null && dtoSO.RmrkArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.RmrkArr.Contains(x.Rmrk));
                }
                else if (!string.IsNullOrEmpty(dtoSO.Rmrk))
                {
                    dbDto = dbDto
        .Where(x => x.Rmrk.ToUpper().StartsWith(dtoSO.Rmrk.ToUpper()));
                }
                if (dtoSO.IsActiveArr != null && dtoSO.IsActiveArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.IsActiveArr.Contains(x.IsActive));
                }
                else if (!string.IsNullOrEmpty(dtoSO.IsActive))
                {
                    dbDto = dbDto
        .Where(x => x.IsActive.ToUpper().StartsWith(dtoSO.IsActive.ToUpper()));
                }
                if (dtoSO.CreIDArr != null && dtoSO.CreIDArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.CreIDArr.Contains(x.CreID));
                }
                else if (!string.IsNullOrEmpty(dtoSO.CreID))
                {
                    dbDto = dbDto
        .Where(x => x.CreID.ToUpper().StartsWith(dtoSO.CreID.ToUpper()));
                }
                if ((DateTime.Compare(dtoSO.CreTime1, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 (DateTime.Compare(dtoSO.CreTime2, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.CreTime1 != default(DateTime) && dtoSO.CreTime2 != default(DateTime))
                {
                    dbDto = dbDto
        .Where(x => x.CreTime >= dtoSO.CreTime1 && x.CreTime <= dtoSO.CreTime2);
                }
                else if ((DateTime.Compare(dtoSO.CreTime1, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.CreTime1 != default(DateTime))
                {
                    dbDto = dbDto
                   .Where(x => x.CreTime >= dtoSO.CreTime1);
                }
                else if ((DateTime.Compare(dtoSO.CreTime2, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.CreTime2 != default(DateTime))
                {
                    dbDto = dbDto
                   .Where(x => x.CreTime <= dtoSO.CreTime2);
                }
                if (dtoSO.ModIDArr != null && dtoSO.ModIDArr.Length > 0)
                {
                    dbDto = dbDto
        .Where(x => dtoSO.ModIDArr.Contains(x.ModID));
                }
                else if (!string.IsNullOrEmpty(dtoSO.ModID))
                {
                    dbDto = dbDto
        .Where(x => x.ModID.ToUpper().StartsWith(dtoSO.ModID.ToUpper()));
                }
                if ((DateTime.Compare(dtoSO.ModTime1, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 (DateTime.Compare(dtoSO.ModTime2, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.ModTime1 != default(DateTime) && dtoSO.ModTime2 != default(DateTime))
                {
                    dbDto = dbDto
        .Where(x => x.ModTime >= dtoSO.ModTime1 && x.ModTime <= dtoSO.ModTime2);
                }
                else if ((DateTime.Compare(dtoSO.ModTime1, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.ModTime1 != default(DateTime))
                {
                    dbDto = dbDto
                   .Where(x => x.ModTime >= dtoSO.ModTime1);
                }
                else if ((DateTime.Compare(dtoSO.ModTime2, DateUtil.ToDate(AppConst.DEFAULT_DATE)) != 0) &&
                 dtoSO.ModTime2 != default(DateTime))
                {
                    dbDto = dbDto
                   .Where(x => x.ModTime <= dtoSO.ModTime2);
                }

            }
            return dbDto;
        }

        private IQueryable<AppConfig> AddFilter(IQueryable<AppConfig> dbDto, string whereSQL)
        {
            if (!string.IsNullOrEmpty(whereSQL))
            {
                whereSQL = whereSQL.Replace('\'', '\"');
                dbDto = dbDto.Where(whereSQL);
            }
            return dbDto;
        }

        private IQueryable<AppConfig> AddOrderBy(IQueryable<AppConfig> dbDto, string orderBy)
        {
            if (!string.IsNullOrEmpty(orderBy))
            { dbDto = dbDto.OrderBy(orderBy.Trim()); }
            else
            {
                dbDto = dbDto.OrderBy(x => x.version);
                dbDto = dbDto.OrderBy(x => x.ConfigKey);

            }
            return dbDto;
        }

        private IQueryable<AppConfig> AddOrderBy(IQueryable<AppConfig> dbDto, string orderBy, string defaultOrderByColumn)
        {
            dbDto = !string.IsNullOrEmpty(orderBy) ? dbDto.OrderBy(orderBy) : dbDto.OrderBy(defaultOrderByColumn);

            return dbDto;
        }

    }
}
