import React, { Component } from "react";
import AdminMenu from "./AdminMenu";
import { Route } from "react-router-dom";
import Employee from "./Employee";
import Departments from "./Departments";
import Clients from "./Clients";
import Engagements from "./Engagements";
import ClientIndustries from "./ClientIndustries";

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
                        <Route path='/admin/employees' component={Employee} />
                        <Route path='/admin/departments' component={Departments} />
                        <Route path='/admin/clients' component={Clients} />
                        <Route path='/admin/engagements' component={Engagements} />
                        <Route path='/admin/client-industries' component={ClientIndustries} />
                    </div>
                </div>
            </div>
        );
    }
}

export default Admin;