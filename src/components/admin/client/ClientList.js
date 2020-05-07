import React, { Component } from "react";
import { Link } from "react-router-dom";

class ClientsList extends Component {
    render() {
        return (
            <div className='text-left'>
                <Link to='/admin/clients' className='btn btn-light btn-sm text-left'>Dashboard</Link>&nbsp;&nbsp;
                <Link to='/admin/clients/add' className='btn btn-light btn-sm'>Add new client</Link>
                <br /><br />
                <h5>Clients list</h5>
                <hr />
                <table className='table table-striped table-bordered table-sm'>
                    <thead className='thead-dark'>
                        <tr>
                            <th scope='row'>Name</th>
                            <th scope='row'>Address</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ClientsList;