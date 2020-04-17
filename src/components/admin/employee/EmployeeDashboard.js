import React, { Component } from "react";
import { Link } from "react-router-dom";

class EmployeeDashboard extends Component {
    render() {
        return (
            <div>
                <div className='text-left'>
                    <Link to='/admin/employees' className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                    <Link to='/admin/employees/list' className='btn btn-light btn-sm text-left'>Employees list</Link>&nbsp;&nbsp;
                    <Link to='/admin/employees/add' className='btn btn-light btn-sm'>Add new employee</Link>
                </div>
                <br />
                <div className="text-justify">
                    <h4>Employees Dashboard</h4>
                </div>
            </div>
        );
    }
}

export default EmployeeDashboard;