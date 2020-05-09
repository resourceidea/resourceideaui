import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class LinesOfServiceDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='lines of service'
                    dashboardLink='/admin/lines-of-service'
                    listLink='/admin/lines-of-service/list'
                    addNewLink='/admin/lines-of-service/add' />
            </div>
        );
    }
}

export default LinesOfServiceDashboard;