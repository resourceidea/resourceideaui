import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class ClientIndustriesDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='client industry'
                    dashboardLink='/admin/client-industries'
                    listLink='/admin/client-industries/list'
                    addNewLink='/admin/client-industries/add' />
            </div>
        );
    }
}

export default ClientIndustriesDashboard;