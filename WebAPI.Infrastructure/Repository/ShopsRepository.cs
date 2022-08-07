using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using WebAPI.Core.Dto.Shops;
using WebAPI.Core.Entities;
using WebAPI.Core.Interfaces.Repositories;
using WebAPI.Infrastructure.Common;
using WebAPI.Infrastructure.Context;

namespace WebAPI.Infrastructure.Repository
{
    public class ShopsRepository : IShopsRepository
    {
        private ApplicationDBContext _applicationDBContext;

        public ShopsRepository(ApplicationDBContext context)
        {
            _applicationDBContext = context;
        }

        public bool AddShops(Shops shops)
        {
            bool _success = false;
            using (MySqlConnection lconn = new MySqlConnection(_applicationDBContext.Database.GetDbConnection().ConnectionString))
            {
                lconn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = lconn;
                    cmd.CommandText = "adm_addShops";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@i_shopName", shops.Shop);
                    cmd.Parameters.AddWithValue("@i_mailChimpListId", shops.MailChimpListID);
                    cmd.Parameters.AddWithValue("@i_Address", shops.Address);
                    cmd.Parameters.AddWithValue("@i_ZipCode", shops.ZipCode);
                    cmd.Parameters.AddWithValue("@i_Location", shops.Location);
                    cmd.Parameters.AddWithValue("@i_ProvinceId", shops.ProvinceId);
                    cmd.Parameters.AddWithValue("@i_CountryId", shops.CountryId);
                    cmd.Parameters.AddWithValue("@i_PhysicalStore", shops.PhysicalStore);

                    cmd.Parameters.AddWithValue("@responseCode", MySqlDbType.Int32);
                    cmd.Parameters["@responseCode"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseMessage", MySqlDbType.VarChar);
                    cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.ExecuteNonQuery();
                    Object objResponseCode = cmd.Parameters["@responseCode"].Value;
                    var responseCode = (Int32)objResponseCode;

                    _success = (responseCode >= 200 && responseCode < 300) ? true : false;
                }
                return _success;
            }
        }
        public bool UpdateShops(int id, Shops shops)
        {
            bool _success = false;
            using (MySqlConnection lconn = new MySqlConnection(_applicationDBContext.Database.GetDbConnection().ConnectionString))
            {
                lconn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = lconn;
                    cmd.CommandText = "adm_updateShops";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@i_Id", id);
                    cmd.Parameters.AddWithValue("@i_shopName", shops.Shop);
                    cmd.Parameters.AddWithValue("@i_mailChimpListId", shops.MailChimpListID);
                    cmd.Parameters.AddWithValue("@i_Address", shops.Address);
                    cmd.Parameters.AddWithValue("@i_ZipCode", shops.ZipCode);
                    cmd.Parameters.AddWithValue("@i_Location", shops.Location);
                    cmd.Parameters.AddWithValue("@i_ProvinceId", shops.ProvinceId);
                    cmd.Parameters.AddWithValue("@i_CountryId", shops.CountryId);
                    cmd.Parameters.AddWithValue("@i_PhysicalStore", shops.PhysicalStore);

                    cmd.Parameters.AddWithValue("@responseCode", MySqlDbType.Int32);
                    cmd.Parameters["@responseCode"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseMessage", MySqlDbType.VarChar);
                    cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.ExecuteNonQuery();

                    Object objResponseCode = cmd.Parameters["@responseCode"].Value;
                    var responseCode = (Int32)objResponseCode;

                    _success = (responseCode >= 200 && responseCode < 300) ? true : false;
                }
                return _success;
            }
        }

        public Shops GetShopDetailById(int id)
        {
            var shopDetail = new Shops();

            using (MySqlConnection lconn = new MySqlConnection(_applicationDBContext.Database.GetDbConnection().ConnectionString))
            {
                lconn.Open();

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = lconn;
                    cmd.CommandText = "adm_getShopDetailById";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@i_Id", id);

                    cmd.Parameters.AddWithValue("@responseCode", MySqlDbType.Int32);
                    cmd.Parameters["@responseCode"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseMessage", MySqlDbType.VarChar);
                    cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output; // from System.Data

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                shopDetail = new Shops()
                                {
                                    Id = id,
                                    Shop = CommonMethods.IsNotNull(reader["Shop"]) ? Convert.ToString(reader["Shop"]) : "",
                                    MailChimpListID = CommonMethods.IsNotNull(reader["MailChimpListId"]) ? Convert.ToString(reader["MailChimpListId"]) : "",
                                    IsDeleted = CommonMethods.IsNotNull(reader["IsDeleted"]) ? Convert.ToBoolean(reader["IsDeleted"]) : false,
                                    Address = CommonMethods.IsNotNull(reader["Address"]) ? Convert.ToString(reader["Address"]) : "",
                                    ZipCode = CommonMethods.IsNotNull(reader["ZipCode"]) ? Convert.ToString(reader["ZipCode"]) : "",
                                    Location = CommonMethods.IsNotNull(reader["Location"]) ? Convert.ToString(reader["Location"]) : "",
                                    ProvinceId = CommonMethods.IsNotNull(reader["ProvinceId"]) ? Convert.ToInt32(reader["ProvinceId"]) : 0,
                                    ProvinceName = CommonMethods.IsNotNull(reader["ProvinceName"]) ? Convert.ToString(reader["ProvinceName"]) : "",
                                    CountryId = CommonMethods.IsNotNull(reader["CountryId"]) ? Convert.ToInt32(reader["CountryId"]) : 0,
                                    CountryName = CommonMethods.IsNotNull(reader["CountryName"]) ? Convert.ToString(reader["CountryName"]) : "",
                                    PhysicalStore = CommonMethods.IsNotNull(reader["PhysicalStore"]) ? Convert.ToInt32(reader["PhysicalStore"]) : 0
                                };
                            }
                        }
                        else
                        {
                            shopDetail = null;
                        }
                    }

                    Object objResponseCode = cmd.Parameters["@responseCode"].Value;
                    var responseCode = (Int32)objResponseCode;

                    Object objResponseMessage = cmd.Parameters["@responseMessage"].Value;
                    var responseMessage = Convert.ToString(objResponseMessage);
                }
                return shopDetail;
            }
        }

        public bool DeleteShopById(int id)
        {
            bool _success = false;
            using (MySqlConnection lconn = new MySqlConnection(_applicationDBContext.Database.GetDbConnection().ConnectionString))
            {
                lconn.Open();
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = lconn;
                    cmd.CommandText = "adm_deleteShopById";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@i_shopId", id);

                    cmd.Parameters.AddWithValue("@isSucess", MySqlDbType.Bit);
                    cmd.Parameters["@isSucess"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseCode", MySqlDbType.Int32);
                    cmd.Parameters["@responseCode"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseMessage", MySqlDbType.VarChar);
                    cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.ExecuteNonQuery();

                    Object objResponseCode = cmd.Parameters["@responseCode"].Value;
                    var responseCode = (Int32)objResponseCode;

                    _success = (responseCode >= 200 && responseCode < 300) ? true : false;
                }
                return _success;
            }
        }

        public ShopListResponseDTO GetShopsList(ShopListRequest shopsListRequest)
        {
            ShopListResponseDTO _response = new ShopListResponseDTO();
            _response.data = new List<Shops>();
            using (MySqlConnection lconn = new MySqlConnection(_applicationDBContext.Database.GetDbConnection().ConnectionString))
            {
                lconn.Open();

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = lconn;
                    cmd.CommandText = "adm_getShopsList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@i_pageIndex", shopsListRequest.pageNo);
                    cmd.Parameters.AddWithValue("@i_pageSize", shopsListRequest.pageSize);

                    cmd.Parameters.AddWithValue("@responseCode", MySqlDbType.Int32);
                    cmd.Parameters["@responseCode"].Direction = ParameterDirection.Output; // from System.Data

                    cmd.Parameters.AddWithValue("@responseMessage", MySqlDbType.VarChar);
                    cmd.Parameters["@responseMessage"].Direction = ParameterDirection.Output; // from System.Data

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _response.TotalRecords = Convert.ToInt32(reader.GetValue(0));
                            }

                            reader.NextResult();

                            while (reader.Read())
                            {
                                Shops shops = new Shops()
                                {
                                    Id = Convert.ToInt32(reader.GetValue(1)),
                                    Shop = CommonMethods.IsNotNull(reader["Shop"]) ? Convert.ToString(reader["Shop"]) : "",
                                    MailChimpListID = CommonMethods.IsNotNull(reader["MailChimpListId"]) ? Convert.ToString(reader["MailChimpListId"]) : "",
                                    IsDeleted = CommonMethods.IsNotNull(reader["IsDeleted"]) ? Convert.ToBoolean(reader["IsDeleted"]) : false,
                                    Address = CommonMethods.IsNotNull(reader["Address"]) ? Convert.ToString(reader["Address"]) : "",
                                    ZipCode = CommonMethods.IsNotNull(reader["ZipCode"]) ? Convert.ToString(reader["ZipCode"]) : "",
                                    Location = CommonMethods.IsNotNull(reader["Location"]) ? Convert.ToString(reader["Location"]) : "",
                                    ProvinceId = CommonMethods.IsNotNull(reader["ProvinceId"]) ? Convert.ToInt32(reader["ProvinceId"]) : 0,
                                    ProvinceName = CommonMethods.IsNotNull(reader["ProvinceName"]) ? Convert.ToString(reader["ProvinceName"]) : "",
                                    CountryId = CommonMethods.IsNotNull(reader["CountryId"]) ? Convert.ToInt32(reader["CountryId"]) : 0,
                                    CountryName = CommonMethods.IsNotNull(reader["CountryName"]) ? Convert.ToString(reader["CountryName"]) : "",
                                    PhysicalStore = CommonMethods.IsNotNull(reader["PhysicalStore"]) ? Convert.ToInt32(reader["PhysicalStore"]) : 0
                                };
                                _response.data.Add(shops);
                            }
                        }
                    }
                }
                return _response;
            }
        }
    }
}
