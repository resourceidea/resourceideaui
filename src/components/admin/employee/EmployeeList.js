import React, { Component } from "react";
import { Link } from "react-router-dom";

class EmployeesList extends Component {
    render() {
        return (
            <div className='text-left'>
                <Link to='/admin/employees' className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                <Link to='/admin/employees/add' className='btn btn-light btn-sm'>Add new employee</Link>
                <br /><br />
                <h3>Employees list</h3>
                <table className='table table-striped table-bordered table-sm'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='row'>First name</th>
                            <th scope='row'>Last name</th>
                            <th scope='row'>Email</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }
}

export default EmployeesList;