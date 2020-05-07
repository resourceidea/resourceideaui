import React, { Component } from "react";
import AdminMenu from "./AdminMenu";
import { Route } from "react-router-dom";
import EmployeeDashboard from "./employee/EmployeeDashboard";
import ClientIndustries from "./clientIndustry/ClientIndustries";
import JobPosition from "./jobPosition/JobPosition";
import LinesOfService from './linesOfService/LinesOfService';
import EmployeeList from "./employee/EmployeeList";
import AddEmployee from "./employee/AddEmployee";
import ClientDashboard from "./client/ClientDashboard";
import ClientsList from "./client/ClientList";
import AddClient from "./client/AddClient";
import DepartmentDashboard from "./department/DepartmentDashboard";
import DepartmentsList from "./department/DepartmentList";
import AddDepartment from "./department/AddDepartment";
import AddEngagement from "./engagement/AddEngagement";
import EngagementsList from "./engagement/EngagementsList";
import EngagementsDashboard from "./engagement/EngagementsDashboard";

class Admin extends Component {
    render() {
        return (
            <div className='container-fluid'>
                <h3> Administration Panel</h3>
                <div className='row'>
                    <div className='col-2'>
                        <AdminMenu />
                    </div>
                    <div className='col-10'>
                        <Route path='/admin/client-industries' component={ClientIndustries} />
                        <Route path='/admin/job-positions' component={JobPosition} />
                        <Route path='/admin/lines-of-service' component={LinesOfService} />

                        {/* Employee admin components */}
                        <Route exact path='/admin/employees' component={EmployeeDashboard} />
                        <Route path='/admin/employees/list' component={EmployeeList} />
                        <Route path='/admin/employees/add' component={AddEmployee} />

                        {/* Client admin components */}
                        <Route exact path='/admin/clients' component={ClientDashboard} />
                        <Route path='/admin/clients/list' component={ClientsList} />
                        <Route path='/admin/clients/add' component={AddClient} />

                        {/* Department admin components */}
                        <Route exact path='/admin/departments' component={DepartmentDashboard} />
                        <Route path='/admin/departments/list' component={DepartmentsList} />
                        <Route path='/admin/departments/add' component={AddDepartment} />

                        {/* Engagements admin components */}
                        <Route exact path='/admin/engagements' component={EngagementsDashboard} />
                        <Route path='/admin/engagements/list' component={EngagementsList} />
                        <Route path='/admin/engagements/add' component={AddEngagement} />

                    </div>
                </div>
            </div>
        );
    }
}

export default Admin;