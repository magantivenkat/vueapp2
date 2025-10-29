/* MRF Changes
Created Date: 22nd September 2022
Created By: Mandar.Khade@amexgbt.com
Team member: Harish.Rame@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 221 */


export default function () {

    const $ = window.jQuery;

    $(function () {

        $('#dataTable').DataTable({
            "order": [[1, "desc"]]
        });

    })
}