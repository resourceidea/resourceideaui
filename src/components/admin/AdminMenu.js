import React, { Component } from "react";
import { Link } from "react-router-dom";

class AdminMenu extends Component {
    render() {
        const adminMenuStyle = {
            textAlign: 'left'
        };

        return (
            <nav>
                <ul className="list-group">
                    <li style={adminMenuStyle} className="list-group-item active">Menu</li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/employees'>Employees</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/departments'>Departments</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/clients'>Clients</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/engagements'>Engagements</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/client-industries'>Client Industries</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/job-positions'>Job Positions</Link>
                    </li>
                    <li style={adminMenuStyle} className="list-group-item">
                        <Link to='/admin/lines-of-service'>Lines Of Service</Link>
                    </li>
                </ul>
            </nav>
        );
    }
}

export default AdminMenu;