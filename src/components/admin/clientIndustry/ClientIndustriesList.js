import React, { Component } from "react";
import AdminListActionBar from "../../common/actionbar/AdminListActionBar";

class ClientIndustriesList extends Component {
    render() {
        return (
            <div className='text-left'>
                <AdminListActionBar
                    page='client industry'
                    dashboardLink='/admin/client-industries'
                    addNewLink='/admin/client-industries/add' />
                <table className='table table-striped table-bordered table-sm'>
                    <thead className='thead-light'>
                        <tr>
                            <th scope='row'>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ClientIndustriesList;