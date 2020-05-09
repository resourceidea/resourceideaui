import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class ClientDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='client'
                    dashboardLink='/admin/clients'
                    listLink='/admin/clients/list'
                    addNewLink='/admin/clients/add' />
            </div>
        );
    }
}

export default ClientDashboard;