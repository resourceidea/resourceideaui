import React, { Component } from "react";
import AdminDashboardActionBar from "../../common/actionbar/AdminDashboardActionBar";

class EmployeeDashboard extends Component {
    render() {
        return (
            <div className='text-left'>
                <AdminDashboardActionBar
                    page='employee'
                    dashboardLink='/admin/employees'
                    listLink='/admin/employees/list'
                    addNewLink='/admin/employees/add' />
            </div>
        );
    }
}

export default EmployeeDashboard;