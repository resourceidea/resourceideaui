import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class DepartmentDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='department'
                    dashboardLink='/admin/departments'
                    listLink='/admin/departments/list'
                    addNewLink='/admin/departments/add' />
            </div>
        );
    }
}

export default DepartmentDashboard;