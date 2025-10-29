<template>
    <div>       
        <div class="form-group">
            <label>Placeholder</label>
            <input v-model="field.placeholder" type="text" class="form-control" />
        </div>
    </div>
</template>

<script>
    export default {
        props: {
            field: {
                type: Object,
                required: true
            }
        }
    };
</script>