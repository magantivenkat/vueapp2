<template>
  <tr>
    <td>{{ label }}</td>
    <td>{{ value }}</td>
  </tr>
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true,
    },
    value: String,
  },
};
</script>