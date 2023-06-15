const chainstoreurl = "http://104.40.5.199:5175/ChainStoreApi";
export const ChainStore = {
    Register: `${chainstoreurl}/RegisterUser`,
    logIn: `${chainstoreurl}/login`,
    StoreLocations: `${chainstoreurl}/availableLocations`,
    ChooseLocation: `${chainstoreurl}/SelectStoreLocation`,
    AvailableProducts: `${chainstoreurl}/GetProducts`,
    ChooseProducts: `${chainstoreurl}/ChooseProduct`,
    CustOrderHist: `${chainstoreurl}/CustOrderHistory`
}