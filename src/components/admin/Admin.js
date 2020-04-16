import React, { Component } from "react";
import AdminMenu from "./AdminMenu";
import { Route } from "react-router-dom";
import EmployeeDashboard from "./employee/EmployeeDashboard";
import Departments from "./department/Departments";
import Clients from "./client/Clients";
import Engagements from "./engagement/Engagements";
import ClientIndustries from "./clientIndustry/ClientIndustries";
import JobPosition from "./jobPosition/JobPosition";
import LinesOfService from './linesOfService/LinesOfService';
import EmployeeList from "./employee/EmployeeList";
import AddEmployee from "./employee/AddEmployee";

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
                        <Route path='/admin/departments' component={Departments} />
                        <Route path='/admin/clients' component={Clients} />
                        <Route path='/admin/engagements' component={Engagements} />
                        <Route path='/admin/client-industries' component={ClientIndustries} />
                        <Route path='/admin/job-positions' component={JobPosition} />
                        <Route path='/admin/lines-of-service' component={LinesOfService} />

                        {/* Employee admin components */}
                        <Route exact path='/admin/employees' component={EmployeeDashboard} />
                        <Route path='/admin/employees/list' component={EmployeeList} />
                        <Route path='/admin/employees/add' component={AddEmployee} />

                    </div>
                </div>
            </div>
        );
    }
}

export default Admin;