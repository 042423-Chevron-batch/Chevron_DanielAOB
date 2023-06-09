using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;


using ChainStoreApiModel;
using ChainStoreApiRepository;

namespace ChainStoreApiBusiness

{
    public class ChainstoreBusinessUtilityLayer
    {





        public Person UserRegistration(Register reg)
        {
            Person p = Mapper.RegisterDtoToPerson(reg);

            RepositoryLayer repo = new RepositoryLayer();

            (bool RegSuccess, Person RegisteredPerson) = repo.Registration(reg);

            if (RegSuccess)
            {
                return RegisteredPerson;
            }
            else
            {
                return null;
            }
        }


        public Person? UserLogin(LogIn login)
        {
            RepositoryLayer repo = new RepositoryLayer();
            (bool LogSuccess, Person LogPerson) = repo.LogIn(login);

            if (LogSuccess)
            {
                return LogPerson;
            }
            else
            {
                return null;
            }
        }

        //Get store locations
        public List<Store> GetStoreLoc()
        {

            RepositoryLayer repo = new RepositoryLayer();

            List<Store> stores = repo.StoreDetails();

            if (stores.Count > 0)
            {

                List<string> locations = stores.Select(store => store.StoreLoc).ToList();
                return stores;
            }
            else
            {
                return null;
            }


        }



        public List<Store>? getStores()
        {
            RepositoryLayer repo = new RepositoryLayer();

            List<Store>? stores = repo.StoreDetails();

            if (stores.Count > 0)
            {
                return stores;

            }
            else
            {
                return null;
            }

        }


        //add customer purchase 

        public bool AddCustomerPurchase(List<Order> orders, Person orderperson)
        {
            RepositoryLayer repo = new RepositoryLayer();

            foreach (Order order in orders)
            {

                bool custOrder = repo.AddToCustomerOrder(order, orderperson);
                if (custOrder)
                {
                    return custOrder;
                }
                else
                {
                    return false;
                }


            }
            //
            return true;
        }


        public List<orderHist>? OrderHist(LogIn login)
        {
            RepositoryLayer repo = new RepositoryLayer();

            List<orderHist> custOrderHis = repo.customerOrderHistory(login);

            if (custOrderHis.Count > 0)
            {
                return custOrderHis;
            }
            else
            {
                return null;
            }

        }







        //show products at selected store using the storeId
        public List<Product> productsInStore(Store prodInstore)
        {
            RepositoryLayer repo = new RepositoryLayer();

            List<Product> prodInStore = repo.ProductDetails(prodInstore);

            if (prodInStore.Count > 0)
            {
                return prodInStore;
            }
            else
            {
                return prodInStore;
            }

        }



        public bool DecreaseInventory(string selectedProduct, int orderedQuant)
        {
            RepositoryLayer repo = new RepositoryLayer();

            bool updatedInventory = repo.UpdateInventory(selectedProduct, orderedQuant);

            if (updatedInventory)
            {
                return updatedInventory;

            }
            else
            {
                return false;
            }
        }

    }


}
