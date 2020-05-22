import React, { Component } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faListAlt,
    faEnvelopeOpen,
    faTimesCircle,
    faInfoCircle,
    faExclamationTriangle,
    faBomb,
    faAngleDoubleLeft,
    faAngleDoubleRight,
    faAngleLeft,
    faAngleRight
} from "@fortawesome/free-solid-svg-icons";


class AdminDashboard extends Component{
    render(){
        return(
            <div className='text-left'>
                <h5>Message Center</h5>
                <hr />
                <div>
                    <table className='table table-striped table-compressed table-bordered table-responsive-lg'>
                        <thead>
                            <tr>
                                <td colSpan='5'>
                                    <div className="btn-group" role="group" aria-label="Pagination">
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleDoubleLeft} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleLeft} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleRight} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleDoubleRight} /></button>
                                    </div>
                                </td>
                            </tr>
                        </thead>
                        <thead>
                            <tr>
                                <th className='w-25'>Date</th>
                                <th className='w-25'>User</th>
                                <th className='w-25'>Category</th>
                                <th className='w-auto'>Level</th>
                                <th className='w-auto'>&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-auto text-center text-dark'><FontAwesomeIcon icon={faBomb} /></td>
                                <td className='w-auto'>
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>View&nbsp;&nbsp;<FontAwesomeIcon icon={faListAlt} /></button>
                                    &nbsp;&nbsp;
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>Mark read&nbsp;<FontAwesomeIcon icon={faEnvelopeOpen} /></button>
                                </td>
                            </tr>
                            <tr>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-auto text-center text-danger'><FontAwesomeIcon icon={faTimesCircle} /></td>
                                <td className='w-auto'>
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>View&nbsp;&nbsp;<FontAwesomeIcon icon={faListAlt} /></button>
                                    &nbsp;&nbsp;
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>Mark read&nbsp;<FontAwesomeIcon icon={faEnvelopeOpen} /></button>
                                </td>
                            </tr>
                            <tr>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-auto text-center text-warning'><FontAwesomeIcon icon={faExclamationTriangle} /></td>
                                <td className='w-auto'>
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>View&nbsp;&nbsp;<FontAwesomeIcon icon={faListAlt} /></button>
                                    &nbsp;&nbsp;
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>Mark read&nbsp;<FontAwesomeIcon icon={faEnvelopeOpen} /></button>
                                </td>
                            </tr>
                            <tr>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-25'>&nbsp;</td>
                                <td className='w-auto text-center text-info'><FontAwesomeIcon icon={faInfoCircle} /></td>
                                <td className='w-auto'>
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>View&nbsp;&nbsp;<FontAwesomeIcon icon={faListAlt} /></button>
                                    &nbsp;&nbsp;
                                    <button type='button' className='btn btn-sm btn-light btn-outline-dark'>Mark read&nbsp;<FontAwesomeIcon icon={faEnvelopeOpen} /></button>
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colSpan='5'>
                                    <div className="btn-group" role="group" aria-label="Pagination">
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleDoubleLeft} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleLeft} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleRight} /></button>
                                        <button type="button" className="btn btn-light"><FontAwesomeIcon icon={faAngleDoubleRight} /></button>
                                    </div>
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        );
    }
}

export default AdminDashboard;