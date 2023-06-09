using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainStoreApiModel;

namespace ChainStoreApiModel
{
    public class Mapper
    {
        public static Person RegisterDtoToPerson(Register dto)
        {
            return new Person(
                Guid.Empty,
                dto.FirstName,
                dto.LastName,
                dto.UserName,
                dto.Email,
                dto.Password
            );
        }
    }



}