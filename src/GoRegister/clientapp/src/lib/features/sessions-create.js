

export default function () {

    const $ = window.jQuery;

    $(function () {
        $("#RegTypeIds").select2();

            $('#CapacityTotal').val(parseInt($('#Capacity').val()) + parseInt($('#CapacityReserved').val()))
            $('#Capacity, #CapacityReserved').on('keyup', function () {
                var capacityValue = parseInt($('#Capacity').val()),
                    capacityReservedValue = parseInt($('#CapacityReserved').val());

                $('#CapacityTotal').val(capacityValue + capacityReservedValue)
            })

            // Create Category
            $('#SaveCategoryButton').on('click', function (e) {
                e.preventDefault()
                var projectId = window.location.pathname.split('/')[3]

                var data = {
                    name: $('#CategoryName').val(),
                    description: $('#CategoryDescription').val(),
                    isSingleSession: $('[name=CategoryLimit]:checked').val()
                }

                $.ajax({
                    url: '/admin/project/' + projectId + '/Sessions/Categories',
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    method: 'POST',
                    data: data,
                    success: function (category) {
                        // add to dropdown list
                        console.log(category)
                        var $categoryList = $('#SessionCategoryId')
                        var selectListHtml = '<option value="' + category.id + '">' + category.name + '</option>'

                        // add client themes to list
                        $categoryList.append(selectListHtml)
                    }
                })

                $('#createCategoryModal').modal('hide')
            })

    })
}