#ifndef _HR_FRAMEUTIL_H_
#define _HR_FRAMEUTIL_H_

#include <memory>
#include <boost/any.hpp>

namespace Hr
{
	template <typename T, typename U>
	inline T HrBoostAnyCast(U& u)
	{
		return boost::any_cast(u);
	}

	template <typename T, typename U>
	inline std::shared_ptr<T> HrCheckPointerCast(U& u)
	{
		BOOST_ASSERT(std::dynamic_pointer_cast<T>(u) == std::static_pointer_cast<T>(u));
		return std::static_pointer_cast<T>(u);
	}

	template <typename T, typename U>
	inline std::shared_ptr<T> HrStaticPointerCast(U& u)
	{
		return std::static_pointer_cast<T>(u);
	}

}

#endif


