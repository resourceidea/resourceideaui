import React, { Component } from "react";
import AdminMenu from "./AdminMenu";
import { Route } from "react-router-dom";
import EmployeeDashboard from "./employee/EmployeeDashboard";
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
import ClientIndustriesDashboard from "./clientIndustry/ClientIndustriesDashboard";
import ClientIndustriesList from "./clientIndustry/ClientIndustriesList";
import AddClientIndustry from "./clientIndustry/AddClientIndustry";
import JobPositionsDashboard from "./jobPosition/JobPositionsDashboard";
import JobPositionsList from "./jobPosition/JobPositionsList";
import AddJobPosition from "./jobPosition/AddJobPosition";
import LinesOfServiceDashboard from "./linesOfService/LinesOfServiceDashboard";
import LinesOfServiceList from "./linesOfService/LinesOfServiceList";
import AddLinesOfService from "./linesOfService/AddLinesOfService";
import AdminDashboard from "./AdminDashboard";

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

                        {/* Admin dashboard */}
                        <Route path='/admin/dashboard' component={AdminDashboard} />

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

                        {/* Client industries admin components */}
                        <Route exact path='/admin/client-industries' component={ClientIndustriesDashboard} />
                        <Route path='/admin/client-industries/list' component={ClientIndustriesList} />
                        <Route path='/admin/client-industries/add' component={AddClientIndustry} />

                        {/* Job positions admin components */}
                        <Route exact path='/admin/job-positions' component={JobPositionsDashboard} />
                        <Route path='/admin/job-positions/list' component={JobPositionsList} />
                        <Route path='/admin/job-positions/add' component={AddJobPosition} />

                        {/* Lines of service admin components */}
                        <Route exact path='/admin/lines-of-service' component={LinesOfServiceDashboard} />
                        <Route path='/admin/lines-of-service/list' component={LinesOfServiceList} />
                        <Route path='/admin/lines-of-service/add' component={AddLinesOfService} />

                    </div>
                </div>
            </div>
        );
    }
}

export default Admin;