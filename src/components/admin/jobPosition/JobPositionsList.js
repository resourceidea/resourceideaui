import React, { Component } from "react";
import AdminListActionBar from "../../common/actionbar/AdminListActionBar";

class JobPositionsList extends Component {
    render() {
        return (
            <div className='text-left'>
                <AdminListActionBar
                    page='job position'
                    dashboardLink='/admin/job-positions'
                    addNewLink='/admin/job-positions/add' />
                <table className='table table-striped table-bordered table-sm'>
                    <thead className='thead-light'>
                        <tr>
                            <th scope='row'>Title</th>
                            <th scope='row'>Department</th>
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

export default JobPositionsList;