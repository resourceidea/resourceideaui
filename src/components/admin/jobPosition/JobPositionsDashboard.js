import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class JobPositionsDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='job position'
                    dashboardLink='/admin/job-positions'
                    listLink='/admin/job-positions/list'
                    addNewLink='/admin/job-positions/add' />
            </div>
        );
    }
}

export default JobPositionsDashboard;